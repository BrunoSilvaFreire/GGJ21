using System;
namespace Common {
    [Serializable]
    public class IntBindable : Bindable<int> { }
    [Serializable]
    public class UIntBindable : Bindable<uint> { }
    [Serializable]
    public class FloatBindable : Bindable<float> { }
}