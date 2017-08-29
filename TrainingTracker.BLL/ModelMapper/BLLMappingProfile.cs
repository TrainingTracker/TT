using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.BLL.ModelMapper
{
    public class BLLMappingProfile : Profile
    {
        public BLLMappingProfile ()
        {
            CreateMap<CrRatingCalcConfig, Common.Entity.CrRatingCalcConfig>();
            CreateMap<CrRatingCalcRangeConfig, Common.Entity.CrRatingCalcRangeConfig>()
                .ForMember(s => s.FeedbackType, opt => opt.MapFrom(s => s.FeedbackTypeId))
                .ForMember(s=>s.CrRatingCalcConfig,opt=>opt.Ignore());
            CreateMap<CrRatingCalcWeightConfig, Common.Entity.CrRatingCalcWeightConfig>()
                .ForMember(s => s.ReviewPointType, opt => opt.MapFrom(s => s.ReviewPointTypeId))
                .ForMember(s => s.CrRatingCalcConfig, opt => opt.Ignore());
        }
    }
}