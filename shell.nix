# shell.nix
# NIXPKGS_ALLOW_INSECURE=1 nix-shell
{ pkgs ? import (fetchTarball "https://github.com/NixOS/nixpkgs/archive/5ed4e25ab58fd4c028b59d5611e14ea64de51d23.tar.gz") {}

}:
with pkgs;
mkShell {
  name = "dotnet-env";
  packages = [ dotnet-sdk_9 avalonia-ilspy assetripper ];
}
