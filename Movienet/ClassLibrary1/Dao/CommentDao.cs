using ClassLibrary1.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Dao
{
    class CommentDao : ICommentDao
    {
        public Comment CreateComment(Comment comment)
        {
            DataModelContainer ctx = new DataModelContainer();
            ctx.Comments.Add(comment);
            ctx.SaveChanges();
            return comment;
        }

        public bool DeleteComment(Comment comment)
        {
            DataModelContainer ctx = new DataModelContainer();
            ctx.Comments.Remove(comment);
            ctx.SaveChanges();
            return true;
        }

        public List<Comment> getAllComments()
        {
            DataModelContainer ctx = new DataModelContainer();
            List<Comment> comments = ctx.Comments.ToList();

            return comments;
        }

        public Comment GetComment(int cid)
        {
            DataModelContainer ctx = new DataModelContainer();
            Comment query = ctx.Comments.Where(c => c.Id == cid).FirstOrDefault();

            return query;
        }

        public Comment UpdateComment(Comment comment)
        {
            DataModelContainer ctx = new DataModelContainer();
            Comment updated = ctx.Comments.Where(c => c.Id == comment.Id).FirstOrDefault();
            updated = comment;
            ctx.SaveChanges();
            return updated;
        }
    }
}
