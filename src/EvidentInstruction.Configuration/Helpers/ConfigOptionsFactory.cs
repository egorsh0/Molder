﻿using EvidentInstruction.Configuration.Infrastructures;
using EvidentInstruction.Configuration.Models;
using EvidentInstruction.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace EvidentInstruction.Configuration.Helpers
{
    public static class ConfigOptionsFactory
    {
        public static IOptions<IEnumerable<ConfigFile>> Create(IConfiguration configuration)
        {
            var config = new List<ConfigFile>();
            var tags = configuration.GetSection(Constants.CONFIG_BLOCK);

            //TODO: если есть секция CONFIG_BLOCK, то все равно падает на Exists

            if (tags.Exists())
            {
                if (tags.GetChildren().Any())
                {
                    foreach (var tag in tags.GetChildren())
                    {
                        config.Add
                        (
                            new ConfigFile
                            {
                                Tag = tag.Key,
                                Parameters = tag.Get<Dictionary<string, object>>()
                            }
                        );
                    }
                }
                else
                {
                    Log.Logger().LogWarning($"In Section \"{Constants.CONFIG_BLOCK}\" empty blocks (tags) in the appsetting file.");
                }
            }
            else
            {
                Log.Logger().LogWarning($"Section \"{Constants.CONFIG_BLOCK}\" was not found in the appsetting file.");
            }

            return Options.Create(config);
        }
    }
}