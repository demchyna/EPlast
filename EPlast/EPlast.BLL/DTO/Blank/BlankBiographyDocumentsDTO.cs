﻿using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Blank;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.Blank
{
   public class BlankBiographyDocumentsDTO
    {
        public int ID { get; set; }
        public string BlobName { get; set; }
        [Required, MaxLength(120)]
        public string FileName { get; set; }
        [Required]
        public int BlankDocumentTypeId { get; set; }
        public BlankBiographyDocumentsTypeDTO BlankBiographyDocumentsTypeDTO { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
