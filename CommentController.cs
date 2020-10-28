using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service
{
    [Route("api/[controller]")]

    public class CommentController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Json(DAL.Comment.Instance.GetCount());
        }
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Json(DAL.Comment.Instance.GetWorkCount(id));
        }
        [HttpPost("page")]
        public ActionResult getPage([FromBody] Model.Page page)
        {
            var result = DAL.Comment.Instance.GetPage(page);
            if (result.Count() == 0)
                return Json(Result.Err("���ؼ�¼��Ϊ0"));
            else
                return Json(Result.Ok(result));
        }
        [HttpPost("workPage")]
        public ActionResult getWorkPge([FromBody] Model.CommentPage page)
        {
            var result = DAL.Comment.Instance.GetWorkPage(page);
            if(result.Count()==0)
                return Json(Result.Err("���ؼ�¼��Ϊ0"));
            else
                return Json(Result.Ok(result));
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var n = DAL.Comment.Instance.Delete(id);
                if (n != 0)
                    return Json(Result.Ok("ɾ���ɹ�"));
                else
                    return Json(Result.Err("commentId������"));
            }catch(Exception ex)
            {
                return Json(Result.Err(ex.Message));
            }
        }
        [HttpPost]
        public ActionResult Post([FromBody] Model.Comment comment)
        {
            comment.commentTime = DateTime.Now;
            try
            {
                int n = DAL.Comment.Instance.Add(comment);
                return Json(Result.Ok("�������۳ɹ�", n));
            }catch(Exception ex)
            {
                if (ex.Message.ToLower().Contains("foreign key"))
                {
                    if (ex.Message.ToLower().Contains("username"))
                        return Json(Result.Err("�Ϸ��û�������Ӽ�¼"));
                    else
                        return Json(Result.Err("����������Ʒ������"));
                }
                else if (ex.Message.ToLower().Contains("null"))
                {
                    return Json(Result.Err("�������ݡ���ƷId���û�������Ϊ��"));
                }
                else
                    return Json(Result.Err(ex.Message));
            }
        }

    }
}