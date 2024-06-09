using System;

namespace Luminescence.Form;

public abstract class FormModel : ICloneable, IEquatable<FormModel>
{
    public object Clone()
    {
        return MemberwiseClone();
    }

    public abstract bool Equals(FormModel? other);
}