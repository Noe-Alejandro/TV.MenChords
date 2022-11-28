﻿using System.Collections.Generic;

namespace TV.MeanChords.Handlers.DiscHandler
{
    public class GetDiscRequest
    {
        public int DiscId { get; set; }
    }

    public class GetDiscResponse
    {
        public int DiscId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DiscImgUrl { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public GetDiscResponseAuthor Author { get; set; }
        public List<Category> Categories { get; set; }
    }

    public class PostDiscRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DiscImgUrl { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int AuthorID { get; set; }
        public List<PostCategory> Categories { get; set; }
    }

    public class PutDiscRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DiscImgUrl { get; set; }
        public double Price { get; set; }
        public int? Amount { get; set; }
        public int AuthorID { get; set; }
        public  AuthorRequest Author { get; set; }
        public List<PostCategory> Categories { get; set; }
    }

    public class PostDiscResponse
    {
        public int DiscId { get; set; }
    }

    public class GetDiscResponseAuthor
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
    }

    public class AuthorRequest
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
    }
}
