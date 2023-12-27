using System;

namespace Luminescence.Dialog;

public class DialogResultEventArgs<TResult> : EventArgs
{
    public TResult Result { get; }

    public DialogResultEventArgs(TResult result)
    {
        Result = result;
    }
}