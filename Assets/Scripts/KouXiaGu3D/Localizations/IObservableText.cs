using System;

namespace KouXiaGu.Localizations
{

    public interface IObservableText
    {

        IDisposable Subscribe(ITextObserver observer);

    }
}