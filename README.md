# BlazorAppCutomAuth

Estudo de autenticação customizada no Blazor, a partir do manual da Microsoft.

Depois de muito sofrer tentando criar um sistema de login com authenticação externa no Blazor Server segue um passo a passo de como esse repo funciona.

Normalmente criamos um sistema de login para ter controle de do que pode ser visto ou executado pelos usuários, então a primeira coisa que precisamos fazer é colocar no Program.cs

```C#
  builder.Services.AddAuthentication();
  builder.Services.AddAuthorizationCore();

```

adicionar nessa ordem o uso dos serviços

```C#
  app.UseAuthentication();
  app.UseAuthorization();
```
Vamos colocar o cascadingParameter no arquivo App.razor

```C#
﻿<CascadingAuthenticationState>
    <Router>
      ...
    </Router>
</CascadingAuthenticationState>
```
mudar o RouteView para AuthorizeRouteView no arquivo App.razor
```C#
  <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
```
```C#
  <AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
```

Continuarei

## referências

[Visão geral] (https://learn.microsoft.com/pt-br/aspnet/core/security/?view=aspnetcore-7.0)

[Autenticação] (https://learn.microsoft.com/pt-br/aspnet/core/blazor/security/?view=aspnetcore-7.0)]

Limitação do Blazor server onde usando token como cascading parameter
https://github.com/dotnet/aspnetcore/issues/18183
não consegui fazer funionar de primeira, mas vale mais uma tentativa

Estudar um pouco nesse site
https://blazorschool.com/tutorial/blazor-server/dotnet7/basic-authentication-764437
