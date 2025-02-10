using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IProductInventorySnapShotService
    {
        public ServiceResponse<IEnumerable<ProductInventorySnapshotDto>> CreateInventorySnapshots(InventorySnapShotSummaryDto snapshotDto);
        public ServiceResponse<ProductInventorySnapshotDto> GetSnapShotByDate(DateTime date);
    }
}
