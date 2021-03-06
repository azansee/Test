using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace BaseManagement
{

    public class CreateUpdateBaseTypeDto
    {
        public Guid? ParentId { get; set; }
        [Required]
        [MaxLength(BaseConsts.MaxCodeLength)]
        public string Code { get; set; }
        [Required]
        [MaxLength(BaseConsts.MaxNameLength)]
        public string Name { get; set; }
        public int Sort { get; set; }
        [MaxLength(BaseConsts.MaxRemarkLength)]
        public string Remark { get; set; }
    }
}