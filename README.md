# TDS171A_2018_1_Des_Web

<h2>Injeção de Dependência (DI) no ASP.NET Core</h2>

É um padrão de projeto que visa facilitar o desacoplamento de componentes, tornando o projeto modular. Nesse padrão a classe que utilizara o recurso não o cria, mas sim o recebe em seu construtor. Essa classe define nos parâmetros do construtor a interface que o objeto a ser injetado tem implementada. Isso facilita a troca do recurso caso seja necessário, pois a outra classe a ser injetada possuirá os mesmos métodos com as mesamas assinaturas.

Essas dependências tem de ter seu tempo de vida estimado no container que a registra. É possível utilizar 3 métodos para configurar esse tempo de vida:
- Transient:
  - É criada uma nova para cada controller e cada service.
- Scoped:
  - Toda vez que uma nova request chega na controller, a dependência injetada é criada, e permanece viva até aquela request ser respondida.
- Singleton:
  - São criadas uma única vez, e a instância criada será utilizada para todos os controller e services que a injetaram.
  
Exemplo dos 3 tipos de tempo de vida  de DI:

``` c#
using System;

namespace teste
{
    public interface IOperacao
    {
        Guid OperacaoId { get; }
    }

    public interface IOperacaoTransient : IOperacao
    {
    }
    public interface IOperacaoScoped : IOperacao
    {
    }
    public interface IOperacaoSingleton : IOperacao
    {
    }
}
```

``` c#
services.AddTransient<IOperationTransient, Operation>();
services.AddScoped<IOperationScoped, Operation>();
services.AddSingleton<IOperationSingleton, Operation>();
```

``` c#
using DependencyInjectionSample.Interfaces;

namespace DependencyInjectionSample.Services
{
    public class OperationService
    {
        public IOperationTransient TransientOperation { get; }
        public IOperationScoped ScopedOperation { get; }
        public IOperationSingleton SingletonOperation { get; }
        public IOperationSingletonInstance SingletonInstanceOperation { get; }

        public OperationService(IOperationTransient transientOperation,
            IOperationScoped scopedOperation,
            IOperationSingleton singletonOperation,
            IOperationSingletonInstance instanceOperation)
        {
            TransientOperation = transientOperation;
            ScopedOperation = scopedOperation;
            SingletonOperation = singletonOperation;
            SingletonInstanceOperation = instanceOperation;
        }
    }
}
```

``` c#
using DependencyInjectionSample.Interfaces;
using DependencyInjectionSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjectionSample.Controllers
{
    public class OperationsController : Controller
    {
        private readonly OperationService _operationService;
        private readonly IOperationTransient _transientOperation;
        private readonly IOperationScoped _scopedOperation;
        private readonly IOperationSingleton _singletonOperation;
        private readonly IOperationSingletonInstance _singletonInstanceOperation;

        public OperationsController(OperationService operationService,
            IOperationTransient transientOperation,
            IOperationScoped scopedOperation,
            IOperationSingleton singletonOperation,
            IOperationSingletonInstance singletonInstanceOperation)
        {
            _operationService = operationService;
            _transientOperation = transientOperation;
            _scopedOperation = scopedOperation;
            _singletonOperation = singletonOperation;
            _singletonInstanceOperation = singletonInstanceOperation;
        }

        public IActionResult Index()
        {
            // viewbag contains controller-requested services
            ViewBag.Transient = _transientOperation;
            ViewBag.Scoped = _scopedOperation;
            ViewBag.Singleton = _singletonOperation;
            ViewBag.SingletonInstance = _singletonInstanceOperation;

            // operation service has its own requested services
            ViewBag.Service = _operationService;
            return View();
        }
    }
}
```
