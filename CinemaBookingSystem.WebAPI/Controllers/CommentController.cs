﻿using AutoMapper;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper, IErrorService errorService)
        {
            _commentService = commentService;
            _errorService = errorService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getall/{id}")]
        public ActionResult Get([FromHeader, Required] string CBSToken, int id)
        {
            //use movie Id to get all the comment of the Movie
            var listComment = _commentService.GetAllByMovie(id);
            var listCommentVm = _mapper.Map<IEnumerable<CommentViewModel>>(listComment);
            return Ok(listCommentVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CBSToken, int id)
        {
            var comment = _commentService.GetById(id);
            if (comment == null) return NotFound();
            else
            {
                var movieVm = _mapper.Map<CommentViewModel>(comment);
                return Ok(movieVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CBSToken, [FromBody] CommentViewModel commentVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var comment = _mapper.Map<Comment>(commentVm);
                    _commentService.Add(comment);
                    _commentService.SaveChanges();
                    return Created("Create successfully", comment);
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error \"{ve.ErrorMessage}\"");
                        }
                    }
                    _errorService.LogError(ex);
                    return BadRequest(ex.InnerException.Message);
                }
                catch (DbUpdateException dbEx)
                {
                    _errorService.LogError(dbEx);
                    return BadRequest(dbEx.InnerException.Message);
                }
                catch (Exception ex)
                {
                    _errorService.LogError(ex);
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost]
        [Route("update")]
        public ActionResult Put([FromHeader, Required] string CBSToken, [FromBody] CommentViewModel commentVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var comment = _mapper.Map<Comment>(commentVm);
                    _commentService.Update(comment);
                    _commentService.SaveChanges();
                    return Ok(comment);
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error \"{ve.ErrorMessage}\"");
                        }
                    }
                    _errorService.LogError(ex);
                    return BadRequest(ex.InnerException.Message);
                }
                catch (DbUpdateException dbEx)
                {
                    _errorService.LogError(dbEx);
                    return BadRequest(dbEx.InnerException.Message);
                }
                catch (Exception ex)
                {
                    _errorService.LogError(ex);
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult Delete([FromHeader, Required] string CBSToken, int id)
        {
            var comment = _commentService.GetById(id);
            bool IsValid = comment != null;
            if (!IsValid) return BadRequest();
            else
            {
                try
                {
                    _commentService.Delete(id);
                    _commentService.SaveChanges();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}