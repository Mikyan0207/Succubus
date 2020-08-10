﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mikyan.Framework.Services;
using Succubus.Commands.Nsfw.Options;
using Succubus.Database.Models;
using Succubus.Services;

namespace Succubus.Commands.Nsfw.Services
{
    public class NsfwService : IService
    {
        public NsfwService(DbService db, BotService botService)
        {
            DbService = db;
            BotService = botService;
        }

        private DbService DbService { get; }

        public BotService BotService { get; }

        public async Task<Cosplayer> GetCosplayerAsync(string name)
        {
            using var uow = DbService.GetDbContext();
            return await uow.Cosplayers.GetCosplayerAsync(name).ConfigureAwait(false);
        }

        public async Task<Set> GetSetAsync(YabaiOptions options)
        {
            using var uow = DbService.GetDbContext();
            return await uow.Sets.GetSetAsync(options).ConfigureAwait(false);
        }

        public async Task<Set> GetSetAsync(string name)
        {
            using var uow = DbService.GetDbContext();
            return await uow.Sets.GetSetAsync(name).ConfigureAwait(false);
        }

        public IEnumerable<Set> GetSets()
        {
            using var uow = DbService.GetDbContext();
            return uow.Sets.GetAll();
        }

        public async Task<(bool, Set)> AddSetAliasAsync(string set, string alias)
        {
            using var uow = DbService.GetDbContext();
            return await uow.Sets.AddAliasAsync(set, alias).ConfigureAwait(false);
        }

        public async Task<(bool, Set)> RemoveSetAliasAsync(string set, string alias)
        {
            using var uow = DbService.GetDbContext();
            return await uow.Sets.RemoveAliasAsync(set, alias).ConfigureAwait(false);
        }
    }
}