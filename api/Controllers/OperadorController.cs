﻿using api.Domain.ViewModels;
using api.Models.Entities;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Controllers
{
    public class OperadorController : BaseApiController
    {

        private IOperadorService _operadorService;
        private ILoginService _loginService;
        private IMapper _mapper;

        public OperadorController(
            ILogger<OperadorController> logger,
            IOperadorService service,
            ILoginService loginService,
            IMapper mapper

        ) :
            base(logger, mapper)
        {
            _mapper = mapper;
            _operadorService = service;
            _loginService = loginService;
        }
        [Route("List")]
        [HttpGet]
        public async Task<IEnumerable<UsuarioResponseViewModel>> GetAll()
        {
            var clientes = await _operadorService.GetOperadores();
            var clientesVM = new List<UsuarioResponseViewModel>();

            foreach (var cliente in clientes)
            {
                clientesVM.Add(_mapper.Map<UsuarioResponseViewModel>(cliente));
            }

            return clientesVM;
        }

        [Route("GetById/{id}")]
        [HttpGet]
        public async Task<UsuarioResponseViewModel> GetById(int id)
        {
            try
            {
                return _mapper.Map<UsuarioResponseViewModel>(await _operadorService.GetById(id));
            }
            catch (Exception ex)
            {
                return new UsuarioResponseViewModel { Error = ex.Message };
            }
        }

        [Route("Update/{id}")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateViewModel operadorVM)
        {
            try
            {
                var operador = _mapper.Map<Operador>(operadorVM);

                operador.Id = id;
                await _operadorService.Update(operador);

                return StatusCode(200, "atualizado com sucesso!");
            }
            catch (AutoMapperMappingException amex)
            {
                return StatusCode(400, amex.InnerException.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _operadorService.Delete(await _operadorService.GetById(id));

                return StatusCode(200, "Deletado com sucesso!");
            }
            catch (AutoMapperMappingException amex)
            {
                return StatusCode(400, amex.InnerException.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}