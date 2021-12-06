#! /usr/bin/env pwsh

$platform = [System.Environment]::OSVersion.platform
if ($platform -eq 4) {
	$default = 1
}
elseif ($platform -le 2) {
	$default = 0
}
else {
	$default = -1
}

$options = @("&Windows", "&Linux")
$choice = $host.UI.PromptForChoice(
	"Compile exe", 
	"What OS to compile the executable for?",
	$options,
	$default
)

$platform = $null
switch ($choice) {
	0 { $platform = "win-x64" }
	1 { $platform = "linux-x64" }
	Default {
		Write-Host "No OS chosen, exiting."
		Exit
	}
}

$sc = $null
switch ($host.UI.PromptForChoice(
	"Self Contained", 
	"Should the executable be runnable without installing .NET Runtime?",
	@("&Yes", "&No"),
	0)) {
	0 { $sc = "true" }
	1 { $sc = "false" }
}

dotnet publish -r $platform --self-contained $sc
