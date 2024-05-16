using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WordChallenge.Core.Interfaces;
using WordChallenge.Core.Services;

namespace WordChallenge.Core.Extensions
{
    public static class WordChallengeServices
    {
        public static IServiceCollection AddWordChallengeServices(this IServiceCollection services)
        {
            services.AddSingleton<ICombinationProvider, StringCombinationService>();
            services.AddSingleton<ITextFileReader, TextReaderService>();
            return services;
        }
    }
}
