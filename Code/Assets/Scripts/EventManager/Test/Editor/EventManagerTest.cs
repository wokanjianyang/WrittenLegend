// using NUnit.Framework;
//
// namespace SDD.Events.Test {
//
//   public class TestOneEvent : SDD.Events.Event {
//     public string Name { get; set; }
//   }
//
//   public class TestTwoEvent : SDD.Events.Event {
//     public string Name { get; set; }
//   }
//
//   [TestFixture]
//   [Category("EventManager")]
//   public class EventManagerTest  {
//
//     private int delegateLookupCount = 0;
//
//     private int testOneEventCount = 0;
//     private string testOneEventName = "";
//
//     private int testTwoEventCount = 0;
//     private string testTwoEventName = "";
//
//     private void OnTestOne(TestOneEvent e) {
//       testOneEventCount += 1;
//       testOneEventName = e.Name;
//     }
//
//     private void OnTestTwo(TestTwoEvent e) {
//       testTwoEventCount += 1;
//       testTwoEventName = e.Name;
//     }
//
//     [SetUp]
//     public void Before() {
//       delegateLookupCount = EventManager.Inst.DelegateLookupCount;
//       testOneEventCount = 0;
//       testOneEventName = "";
//       testTwoEventCount = 0;
//       testTwoEventName = "";
//     }
//
//     [TearDown]
//     public void After() {
//       // ensure cleanup
//       EventManager.Inst.RemoveListener<TestOneEvent>(OnTestOne);
//       EventManager.Inst.RemoveListener<TestTwoEvent>(OnTestTwo);
//       Assert.IsTrue(EventManager.Inst.DelegateLookupCount == delegateLookupCount);
//     }
//
//     [Test]
//     public void InstPropertyGetterCreatesEventManager() {
//       Assert.IsTrue(EventManager.Inst != null);
//       Assert.IsTrue(EventManager.Inst.GetType() == typeof(EventManager));
//     }
//
//     [Test]
//     public void AddListenerIncrementsLookupCountOncePer() {
//       EventManager.Inst.AddListener<TestOneEvent>(OnTestOne);
//       Assert.IsTrue(EventManager.Inst.DelegateLookupCount == delegateLookupCount + 1);
//
//       EventManager.Inst.AddListener<TestOneEvent>(OnTestOne);
//       Assert.IsTrue(EventManager.Inst.DelegateLookupCount == delegateLookupCount + 1);
//     }
//
//     [Test]
//     public void RemoveListenerDecrementsLookupCountAlways() {
//       EventManager.Inst.AddListener<TestOneEvent>(OnTestOne);
//       EventManager.Inst.AddListener<TestOneEvent>(OnTestOne);
//       EventManager.Inst.AddListener<TestOneEvent>(OnTestOne);
//       Assert.IsTrue(EventManager.Inst.DelegateLookupCount == delegateLookupCount + 1);
//
//       EventManager.Inst.RemoveListener<TestOneEvent>(OnTestOne);
//       Assert.IsTrue(EventManager.Inst.DelegateLookupCount == delegateLookupCount);
//     }
//
//     [Test]
//     public void RemoveListenerHandlesNoListeners() {
//       Assert.IsTrue(testOneEventCount == 0);
//       Assert.IsTrue(EventManager.Inst.DelegateLookupCount == delegateLookupCount);
//
//       EventManager.Inst.RemoveListener<TestOneEvent>(OnTestOne);
//       Assert.IsTrue(testOneEventCount == 0);
//       Assert.IsTrue(EventManager.Inst.DelegateLookupCount == delegateLookupCount);
//     }
//
//     [Test]
//     public void RaiseInvokes() {
//       EventManager.Inst.AddListener<TestOneEvent>(OnTestOne);
//       EventManager.Inst.Raise(new TestOneEvent() { Name="One A" });
//       Assert.IsTrue(testOneEventCount == 1);
//       Assert.IsTrue(testOneEventName == "One A");
//
//       EventManager.Inst.Raise(new TestOneEvent() { Name="One B" });
//       Assert.IsTrue(testOneEventCount == 2);
//       Assert.IsTrue(testOneEventName == "One B");
//     }
//
//     [Test]
//     public void RaiseInvokesCorrectDelegate() {
//       EventManager.Inst.AddListener<TestOneEvent>(OnTestOne);
//       EventManager.Inst.AddListener<TestTwoEvent>(OnTestTwo);
//       EventManager.Inst.Raise(new TestTwoEvent() { Name="Two A" });
//       Assert.IsTrue(testTwoEventCount == 1);
//       Assert.IsTrue(testTwoEventName == "Two A");
//       Assert.IsTrue(testOneEventCount == 0);
//       Assert.IsTrue(testOneEventName == "");
//     }
//
//     [Test]
//     public void RaiseHandlesNoListeners() {
//       Assert.IsTrue(testOneEventCount == 0);
//       Assert.IsTrue(EventManager.Inst.DelegateLookupCount == delegateLookupCount);
//
//       EventManager.Inst.Raise(new TestOneEvent() { Name="One A" });
//       Assert.IsTrue(testOneEventCount == 0);
//       Assert.IsTrue(EventManager.Inst.DelegateLookupCount == delegateLookupCount);
//     }
//   }
// }
//
