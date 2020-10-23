using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCompany.Crm.TechnicalStuff.Crud.Api;
using MyCompany.Crm.TechnicalStuff.Crud.DataAccess;

namespace MyCompany.Crm.Contacts.Groups.OldApi
{
    [ApiController]
    [Route("/api/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly ContactsCrudDao _dao;

        public GroupsController(ContactsCrudDao dao) => _dao = dao;

        [HttpGet("get")]
        public Task<ActionResult<Group>> Get(Guid id) => _dao
            .Read<Group>(id, query => query
                .Include(group => group.Tags))
            .ToOkResult();

        [HttpGet("search")]
        public IAsyncEnumerable<Group> Search(string name = null, int skip = 0, int take = 20) => _dao
            .Read<Group>(query => query
                .Include(group => group.Tags)
                .Where(group => name == null || group.Name.Contains(name))
                .Skip(skip)
                .Take(take));

        [HttpPost("update")]
        public Task<ActionResult<Group>> Update(Guid id, Group group) => _dao
            .Update(id, group)
            .ToOkResult();

        [HttpPost("delete")]
        public Task<OkResult> Delete(Guid id) => _dao
            .Delete<Group>(id, DeletePolicy.Soft)
            .ToOkResult();
        
        [HttpPost("add-tag")]
        public Task<OkResult> AddTag(Guid groupId, Guid tagId) => _dao
            .Update<Group>(groupId,
                query => query.Include(group => group.Tags),
                group =>
                {
                    if (group.Tags.Any(tag => tag.TagId == tagId))
                        return;
                    group.Tags.Add(new GroupTag
                    {
                        GroupId = groupId,
                        TagId = tagId
                    });
                })
            .ToOkResult();

        [HttpPost("remove-tag")]
        public Task<OkResult> RemoveTag(Guid groupId, Guid tagId) => _dao
            .Update<Group>(groupId,
                query => query.Include(group => group.Tags),
                group =>
                {
                    var tagToRemove = group.Tags.FirstOrDefault(tag => tag.TagId == tagId);
                    if (tagToRemove is null)
                        return;
                    group.Tags.Remove(tagToRemove);
                })
            .ToOkResult();
    }
}