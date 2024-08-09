﻿using DocumentFormat.OpenXml.Bibliography;
using VAVS_Client.Data;
using VAVS_Client.Models;

namespace VAVS_Client.Services.Impl
{
    public class TaxPersonImageServiceImpl : AbstractServiceImpl<TaxPersonImage>, TaxPersonImageService
    {

        private readonly ILogger<TaxPersonImageServiceImpl> _logger;

        public TaxPersonImageServiceImpl(VAVSClientDBContext context, ILogger<TaxPersonImageServiceImpl> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public bool SaveTaxPersonImage(TaxPersonImage taxPesonImage)
        {
            taxPesonImage.IsDeleted = false;
            taxPesonImage.CreatedBy = 1;
            taxPesonImage.CreatedDate = DateTime.Now;
            return Create(taxPesonImage);
        }

        public bool UpdateTaxPersonImage(TaxPersonImage taxPesonImage)
        {
            taxPesonImage.IsDeleted = false;
            taxPesonImage.CreatedBy = 1;
            taxPesonImage.CreatedDate = DateTime.Now;
            return Update(taxPesonImage);
        }

        public TaxPersonImage GetTaxPersonImageByCarNumber(string carNumber)
        {
            return _context.TaxPersonImages.FirstOrDefault(taxPersonImage => taxPersonImage.CarNumber == carNumber.Trim());
        }

        public TaxPersonImage GetTaxPersonImageByPersonalDetailPkId(int personalDetailPkId)
        {
            return _context.TaxPersonImages.FirstOrDefault(taxPersonImage => taxPersonImage.PersonalDetailPkid == personalDetailPkId);
        }

        public TaxPersonImage GetTaxPersonImageByPersonalDetailPkIdAndCarNumber(int personalDetailPkId, string carNumber)
        {
           return _context.TaxPersonImages
        .AsNoTracking()
        .FirstOrDefault(taxPersonImage =>
            taxPersonImage.PersonalDetailPkid == personalDetailPkId &&
            taxPersonImage.CarNumber == carNumber);
            //return _context.TaxPersonImages.FirstOrDefault(taxPersonImage => taxPersonImage.PersonalDetailPkid == personalDetailPkId && taxPersonImage.CarNumber == carNumber);
        }

        public bool HardDeleteTaxPersonImage(int personalDetailPkId, string carNumber)
        {
            TaxPersonImage taxPersonImage = GetTaxPersonImageByPersonalDetailPkIdAndCarNumber(personalDetailPkId, carNumber);
            if (taxPersonImage != null)
            {
                HardDelete(taxPersonImage);
            }
            return true;
        }
    }
}
