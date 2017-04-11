using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public interface IReader<TResult>
    {
        TResult Read();
    }

    public interface IReader<TResult, T>
    {
        TResult ReadMap(T source);
    }

    public interface IReader<TResult, T1, T2>
    {
        TResult Read(T1 s1, T2 s2);
    }

    public interface IReader<TResult, T1, T2, T3>
    {
        TResult Read(T1 s1, T2 s2, T3 s3);
    }


    public interface IWriter<TContent>
    {
        void Write(TContent item);
    }

    public interface IWriter<TContent, T1>
    {
        void Write(TContent item, T1 t1);
    }

    public interface IReaderWriter<TR, TW> : IReader<TR>, IWriter<TW, string>
    {
    }



}
