using System;
using System.Threading.Tasks;
using AutoMapper;
using Apex.Core;
using Apex.Core.Entities.FrontE;
using Apex.DAL.Abstracts;
using Apex.Service.Services.Base;
using Apex.Service.ViewModels.Shop;

namespace Apex.Service.Services.Content
{
    public class SliderService : BaseService
    {
        public ISliderRepository SliderRepository { get; }

        public SliderService(RequestInfo info, IUnitOfWork unitOfWork, ISliderRepository sliderRepository) : base(unitOfWork, info)
        {
            SliderRepository = sliderRepository;
        }

        public async Task<ServiceResult<SliderViewModel>> Create(SliderViewModel model)
        {

            var res = new ServiceResult<SliderViewModel>();
            var slider = Mapper.Map<Slider>(model);

            SliderRepository.Insert(slider);

            try
            {
                await UnitOfWork.SaveChangesAsync();

            }
            catch (Exception)
            {
                res.ServerError();
            }

            res.Result = Mapper.Map<SliderViewModel>(slider);
            return res;
        }

        public async Task<ServiceResult<SliderViewModel>> Update(long id, SliderViewModel model)
        {
            var res = new ServiceResult<SliderViewModel>();

            var slider = SliderRepository.OneAsset(id);
            if (slider == null)
            {
                res.NotFound = true;
                return res;
            }

            Mapper.Map(model, slider);

            SliderRepository.ClearPictures(slider.Id);

            SliderRepository.Update(slider);
            try
            {
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                res.ServerError();
            }

            res.Result = model;
            return res;
        }

        public async Task<ServiceResult<Slider>> Delete(long id)
        {
            var res = new ServiceResult<Slider>();

            var slider = SliderRepository.OneAsset(id);
            if (slider == null)
            {
                res.NotFound = true;
                return res;
            }

            SliderRepository.Delete(slider);

            await UnitOfWork.SaveChangesAsync();
            res.Result = slider;
            return res;
        }
    }
}
