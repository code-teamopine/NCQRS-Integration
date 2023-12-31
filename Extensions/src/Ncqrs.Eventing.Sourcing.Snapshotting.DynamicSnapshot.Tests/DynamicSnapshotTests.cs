﻿using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Ncqrs.Domain;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Ncqrs.Eventing.Sourcing.Snapshotting.DynamicSnapshot;
using Xunit;

namespace Ncqrs.Eventing.Sourcing.Snapshotting.DynamicSnapshot.Tests
{

    public class SnapshotableProxyTests : IDisposable
    {
        private const string ChangedTitle = "ChangedTitle";

        private const string OriginalTitle = "OriginalTitle";

        private const string SerializedSnapshotFileName = "snapshot.bin";

        private IWindsorContainer _container;

        private readonly Guid AssemblyVersionGuid = Guid.Parse("938bab08-4f95-430f-b1b7-2200ae4085d5");

        [Fact]
        public void CanBuildDynamicSnapshotAssembly()
        {
            var assembly = _container.Resolve<IDynamicSnapshotAssembly>();
            var snapshotTypesCount = assembly.ActualAssembly.GetTypes().Length;
            Assert.Equal(3, snapshotTypesCount);
        }

        [Fact]
        public void CanChangeAssemblyVersionGuidTest()
        {
            var asm = _container.Resolve<IDynamicSnapshotAssembly>();
            var MVID = asm.ActualAssembly.GetModule("DynamicSnapshot.dll").ModuleVersionId;
            Assert.Equal(AssemblyVersionGuid, MVID);
        }

        [Fact]
        public void CanCreateSnapshotableAggregate()
        {
            var proxy = _container.Resolve<Foo>();
            Assert.NotNull(proxy);
        }

        [Fact]
        public void CanCreateSnapshot()
        {
            dynamic proxy = _container.Resolve<Foo>();
            var snapshot = proxy.CreateSnapshot();
            Assert.NotNull(snapshot);
        }

        [Fact]
        public void CanProperlyRestoreFormDynamicSnapshot()
        {
            //  We assert in each step!!!

            dynamic proxy = _container.Resolve<Foo>();
            proxy.ChangeTitle(OriginalTitle);
            Assert.Equal(OriginalTitle, proxy.Tittle);

            var snapshot = proxy.CreateSnapshot();
            Assert.Equal(OriginalTitle, proxy.Tittle);

            proxy.ChangeTitle(ChangedTitle);
            Assert.Equal(ChangedTitle, proxy.Tittle);

            proxy.RestoreFromSnapshot(snapshot);
            Assert.Equal(OriginalTitle, proxy.Tittle);
        }

        [Fact]
        public void CanRestoreFromSerializedSnapshot()
        {
            if (!File.Exists(SerializedSnapshotFileName))
                return;

            var proxy = _container.Resolve<Foo>();
            object snapshot = DeserializeSnapshot();
            proxy.RestoreFromSnapshot(snapshot);

            Assert.Equal(OriginalTitle, proxy.Tittle);
        }

        public SnapshotableProxyTests()
        {
            System.Diagnostics.Debug.Write("Initializing Windsor container...");
            var target = Assembly.LoadFrom("Ncqrs.Eventing.Sourcing.Snapshotting.DynamicSnapshot.Tests.dll");
            _container = new WindsorContainer();
            _container.AddFacility("Ncqrs.DynamicSnapshot", new DynamicSnapshotFacility(target));
            _container.Register(Component.For<Foo>().AsSnapshotable());
            System.Diagnostics.Debug.WriteLine("Done!");
        }

        
        public void Dispose()
        {
            if (File.Exists(SerializedSnapshotFileName))
                File.Delete(SerializedSnapshotFileName);

            dynamic proxy = _container.Resolve<Foo>();
            proxy.ChangeTitle(OriginalTitle);
            Assert.Equal(OriginalTitle, proxy.Tittle);

            object snapshot = proxy.CreateSnapshot();
            SerializeSnapshot(snapshot);

            System.Diagnostics.Debug.WriteLine("Disposing Windsor container...");
            _container.Dispose();
        }

        private object DeserializeSnapshot()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, eventArgs) =>
            {
                if (eventArgs.Name.Contains("DynamicSnapshot"))
                    return Assembly.LoadFrom("DynamicSnapshot.dll");
                return null;
            };

            using (FileStream stream = new FileStream(SerializedSnapshotFileName, FileMode.Open))
            {
                var bf = new BinaryFormatter();
                return bf.Deserialize(stream);
            }
        }

        private void SerializeSnapshot(object snapshot)
        {
            if (File.Exists(SerializedSnapshotFileName))
                File.Delete(SerializedSnapshotFileName);

            using (Stream stream = File.OpenWrite(SerializedSnapshotFileName))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, snapshot);
            }
        }

    }

    [DynamicSnapshot]
    public class Foo : AggregateRoot
    {
        private string _title;
        public string Tittle { get { return _title; } }

        public Foo() { }
        private Foo(string title) { _title = title; }
        public static Foo CreateNew(string title) { return new Foo(title); }

        public void ChangeTitle(string newTitle) { _title = newTitle; }
    }

    [DynamicSnapshot]
    public class ParentSnapshotableObject : AggregateRoot
    {
        protected string _name = "ParentSnapshotableObject";
        private readonly bool _isActive = true;

        [ExcludeFromSnapshot]
        private int _excludedInt = -123;

        [ExcludeFromSnapshot]
        private DateTime _excludedDate = DateTime.Now;

    }

    [DynamicSnapshot]
    public class SnapshotableObject : ParentSnapshotableObject
    {
        protected string _name = "SnapshotableObject";
        private int _penchoInt = 1245;

        [ExcludeFromSnapshot]
        private bool _excludedBool;
    }

    public class ParentNonSnapshotableObect : AggregateRoot
    {
        private string _name = "ParentNonSnapshotableObect";
        DateTime _date = DateTime.Now;
    }

    public class NonSnapshotableObject : ParentNonSnapshotableObect
    {
        private string _name = "NonSnapshotableObject";
        DateTime _date = DateTime.Now;
    }
}
