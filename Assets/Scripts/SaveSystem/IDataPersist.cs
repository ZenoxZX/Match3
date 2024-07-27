using System;

namespace SaveSystem
{
    public interface IDataPersist
    {
        SGuid SGuid { get; }
        object GetData();
        void SetData(object data);
    }
}