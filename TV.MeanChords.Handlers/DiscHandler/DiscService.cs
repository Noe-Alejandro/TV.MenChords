﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.MeanChords.Data.Db.Context.DiscosChowell;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.DiscHandler
{
    public class DiscService : IDiscService
    {
        private UoWDiscosChowell UoWDiscosChowell { get; set; }
        public static DiscService Create() => new DiscService();
        public DiscService()
        {
            UoWDiscosChowell = UoWDiscosChowell.Create();
        }

        public void Dispose()
        {
            UoWDiscosChowell.Dispose();
            UoWDiscosChowell = null;
        }

        private List<Category> MapCategories(Disc disc)
        {
            List<Category> categories = new List<Category>();
            foreach (var category in disc.DiscTag)
            {
                categories.Add(new Category { Name = category.Tag.Name });
            }
            return categories;
        }

        private bool ValidateParams(Disc request)
        {
            if (String.IsNullOrEmpty(request.Name))
                return false;
            if (String.IsNullOrEmpty(request.Description))
                return false;
            if (String.IsNullOrEmpty(request.DiscImgUrl))
                return false;
            if (request.Price < 0)
                throw new Exception("El precio no puede ser menor a 0");
            if(request.AuthorId == 0)
                throw new Exception("No se envío el identificador del autor o no se encuentra registrado en el sistema");
            return true;
        }

        public ResponseBase<GetDiscResponse> GetDisc(GetDiscRequest request)
        {
            var disc = UoWDiscosChowell.DiscRepository.Get(x => x.DiscId.Equals(request.DiscId)).FirstOrDefault();
            if (disc == null)
                throw new Exception("No se encontró el disco con el ID proporcionado");
            return ResponseBase<GetDiscResponse>.Create(new GetDiscResponse
            {
                Name = disc.Name,
                Description = disc.Description,
                DiscImgUrl = disc.DiscImgUrl,
                Price = disc.Price,
                Amount = disc.Amount,
                Author = new GetDiscResponseAuthor { FullName = disc.Author.FullName, ShortName = disc.Author.ShortName},
                Categories = MapCategories(disc)
            });
        }

        public ResponseBase<List<GetDiscResponse>> GetDiscByTitle(string Title)
        {
            var discLst = UoWDiscosChowell.DiscRepository.Get(x => x.Name.Contains(Title)).ToList();
            if (discLst == null)
                throw new Exception("No se encontraron discos con ese resultado");
            var responseLst = new List<GetDiscResponse>();
            foreach (var disc in discLst)
            {
                responseLst.Add(new GetDiscResponse
                {
                    Name = disc.Name,
                    Description = disc.Description,
                    DiscImgUrl = disc.DiscImgUrl,
                    Price = disc.Price,
                    Amount = disc.Amount,
                    Author = new GetDiscResponseAuthor { FullName = disc.Author.FullName, ShortName = disc.Author.ShortName },
                    Categories = MapCategories(disc)
                });
            }
            return ResponseBase<List<GetDiscResponse>>.Create(responseLst);
        }

        public ResponseBase<List<GetDiscResponse>> GetDiscByCategory(int CategoryId)
        {
            var discLst = UoWDiscosChowell.DiscTagRepository.Get(x => x.TagId.Equals(CategoryId)).ToList();
            if (discLst == null)
                throw new Exception("No se encontraron discos con ese resultado");
            var responseLst = new List<GetDiscResponse>();
            foreach (var disc in discLst)
            {
                responseLst.Add(new GetDiscResponse
                {
                    Name = disc.Disc.Name,
                    Description = disc.Disc.Description,
                    DiscImgUrl = disc.Disc.DiscImgUrl,
                    Price = disc.Disc.Price,
                    Amount = disc.Disc.Amount,
                    Author = new GetDiscResponseAuthor { FullName = disc.Disc.Author.FullName, ShortName = disc.Disc.Author.ShortName },
                    Categories = MapCategories(disc.Disc)
                });
            }
            return ResponseBase<List<GetDiscResponse>>.Create(responseLst);
        }

        public ResponseBase<PostDiscResponse> PostDisc(PostDiscRequest request)
        {
            if (request == null)
                throw new Exception("Favor de mandar los datos del disco");
            Disc disc = new Disc
            {
                Name = request.Name,
                Description = request.Description,
                DiscImgUrl = request.DiscImgUrl,
                Price = request.Price,
                Amount = request.Amount,
                AuthorId = request.AuthorID,
                Status = true,
                CreatedDate = DateTime.Now,
                ModificationDate = DateTime.Now
            };
            ValidateParams(disc);
            UoWDiscosChowell.DiscRepository.Insert(disc);
            if (request.Categories != null) {
                foreach (var category in request.Categories)
                {
                    UoWDiscosChowell.DiscTagRepository.Insert(new DiscTag { DiscId = disc.DiscId, TagId = category.CategoryID});
                }
            }
            UoWDiscosChowell.Save();
            return ResponseBase<PostDiscResponse>.Create(new PostDiscResponse { DiscId = disc.DiscId });
        }

        public ResponseBase<List<GetDiscResponse>> GetAllDisc()
        {
            var disclst = UoWDiscosChowell.DiscRepository.GetAll().ToList();
            if (disclst == null || disclst.Count == 0)
                throw new Exception("No hay discos en la db");
            var responseLst = new List<GetDiscResponse>();
            foreach (var disc in disclst)
            {
                responseLst.Add(new GetDiscResponse
                {
                    Name = disc.Name,
                    Description = disc.Description,
                    DiscImgUrl = disc.DiscImgUrl,
                    Price = disc.Price,
                    Amount = disc.Amount,
                    Author = new GetDiscResponseAuthor { FullName = disc.Author.FullName, ShortName = disc.Author.ShortName },
                    Categories = MapCategories(disc)
                });
            }
            return ResponseBase<List<GetDiscResponse>>.Create(responseLst);
        }
    }
}
