using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAPIStudy.Models;

namespace WebAPIStudy.Controllers
{
    /// <summary>
    /// 学生资源的集合 行为就是Action
    /// </summary>
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        //C R U D
        /// <summary>
        /// GET(查询列表) / students/ return list
        /// </summary>
        [Route("Get")]
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return Storages.Students;
        }

        /// <summary>
        /// GET(查询单个) / students/zhaoheihei return entity
        /// </summary>
        [Route("Get")]
        [HttpGet]
        public Student Get(string item)
        {
            return Storages.Students.FirstOrDefault(s => s.Name.Equals(item, StringComparison.InvariantCultureIgnoreCase));
        }
        /// <summary>
        /// POST(新增) / students /
        /// </summary>
        /// <param name="entity"></param>
        [HttpPost]
        public void Post(Student entity)
        {
            //ToList会生成新的对象。
            //Storages.Students.Append(entity);
            var list = Storages.Students as IList<Student>;
            entity.Id = Storages.Students.Max(t => t.Id) + 1;
            list.Add(entity);
        }

        /// <summary>
        /// PUT(更新) / students /zhaoxiaohei 
        /// </summary>
        /// <param name="entity"></param>
        [HttpPut]
        public void Put([FromUri] string item, [FromBody] Student entity)
        {
            //ToList会生成新的对象。
            //Storages.Students.Append(entity);
            //var list = Storages.Students as IList<Student>;
            //entity.Id = Storages.Students.Max(t => t.Id) + 1;
            //list.Add(entity);
            Delete(item);
            Post(entity);
        }
        /// <summary>
        /// DELETE(删除) / students /zhaoxiaohei 
        /// </summary>
        [HttpDelete]
        public void Delete([FromUri] string item)
        {
            var entity = Get(item);
            var list = Storages.Students as IList<Student>;
            list.Remove(entity);
        }
    }
}