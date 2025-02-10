using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Services.EntityDtos;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Services.DTOs
{
    public class ProductInventorySnapshotDto : FullAuditedEntityDto<int>
    {
        public string? Inventory { get; set; }
        public SnapShotEnum? SnapShotType { get; set; }

    }
}