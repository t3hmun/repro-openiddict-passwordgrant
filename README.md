# repro-openiddict-passwordgrant

## Problem

The token endpoint stopped working for me when my code switched from `OpenIddict 1.0.0-alpha2-0410` to `...-0417`.

However token endpoint does work on a fresh project in `0417`.

It turns `0417` fails when a teminal middleware is added at the end of the config, it shows the terminal message instead of the token.

```csharp
  app.Run(async context =>
  {
      await context.Response.WriteAsync("how is this relevant?");
  });
```

## Running the Repro

Needs a `(localdb)\MSSQLLocalDB` running with an empty database called 

* `repropasswordgrant0410` for the `master` branch, which uses version `0410` demonstrates intended behaviour
* `repropasswordgrant0417` for the `alpha2-0417` branch that repros my problem, not showing the token. If you delete the `app.Run` line it works fine.

I've put migrate and default user creation in the startup code, the default page `api/test/token` makes a request to the `auth/token` endpoint and displays the result.
