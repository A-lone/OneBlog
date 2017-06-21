using Microsoft.AspNetCore.Mvc;
using One.Mock.Data;
using System.Collections.Generic;

namespace One.Mock.Repositories
{
    public interface IDataEventRecordRepository
    {
        List<DataEventRecord> GetAll();
        DataEventRecord Get(long id);

        void Post(DataEventRecord dataEventRecord);

        void Put(long id, [FromBody]DataEventRecord dataEventRecord);

        void Delete(long id);
    }
}