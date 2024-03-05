using System;

namespace Luminescence.Form;

public class FormBaseModel : ICloneable
{
    public object Clone()
    {
        return MemberwiseClone();
    }
}