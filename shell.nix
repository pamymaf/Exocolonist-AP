# shell.nix
# NIXPKGS_ALLOW_INSECURE=1 nix-shell
with import <nixpkgs> { };

mkShell {
  name = "dotnet-env";
  packages = [ dotnet-sdk_9 avalonia-ilspy ];
}