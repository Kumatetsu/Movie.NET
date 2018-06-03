using ModelMovieNet;
using ModelMovieNet.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelMovieNet.Dao
{
    class CommentDao : ICommentDao
    {
        public Comment CreateComment(Comment comment)
        {
            DataModelContainer ctx = new DataModelContainer();
            Console.WriteLine("comment in create: " + comment.Message);
            ctx.CommentSet.Add(comment);
            ctx.SaveChanges();
            return comment;
        }

        public bool DeleteComment(Comment comment)
        {
            DataModelContainer ctx = new DataModelContainer();
            Comment toDelete = ctx.CommentSet.Where(c => c.Id == comment.Id).FirstOrDefault();
            ctx.CommentSet.Remove(toDelete);
            ctx.SaveChanges();
            return true;
        }

        public List<Comment> getAllComments()
        {
            DataModelContainer ctx = new DataModelContainer();
            return ctx.CommentSet.ToList();
        }

        public Comment GetComment(int cid)
        {
            DataModelContainer ctx = new DataModelContainer();
            return ctx.CommentSet.Where(c => c.Id == cid).FirstOrDefault();
        }

        public Comment UpdateComment(Comment comment)
        {
            Console.WriteLine("Comment passed to update: " + comment.ToString());
            DataModelContainer ctx = new DataModelContainer();
            Comment toUpdate = ctx.CommentSet.Where(c => c.Id == comment.Id).FirstOrDefault();
            Console.WriteLine("In UpdateComment, return of update method: " + toUpdate.ToString());
            toUpdate.Message = comment.Message;
            toUpdate.Movie = comment.Movie;
            toUpdate.Note = comment.Note;
            toUpdate.User = comment.User;
            if (toUpdate.Equals(comment))
            {
                Console.WriteLine("Update ok");
                ctx.SaveChanges();
                return toUpdate;
            }
            else
            {
                throw new Exception("Update comment failed");
            }
        }
    }
}
