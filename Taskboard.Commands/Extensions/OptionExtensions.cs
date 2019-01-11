using System;
using Optional;

namespace Taskboard.Commands.Extensions
{
    public static class OptionExtensions
    {
        public static TException ExceptionOrFailure<TValue, TException>(this Option<TValue, TException> option)
        {
            return option.Match(value => throw new InvalidOperationException("Failure"), error => error);
        }
    }
}