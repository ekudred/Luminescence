using System;

namespace Luminescence.Form;

public class FormBaseModel : ICloneable, IEquatable<FormBaseModel>
{
    public object Clone()
    {
        return MemberwiseClone();
    }

    public virtual bool Equals(FormBaseModel? other)
    {
        throw new NotImplementedException();
    }
}