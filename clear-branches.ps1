<#
.SYNOPSIS
    Deletes all locally merged Git branches, but ensures you’re on `master` before any deletion.

.DESCRIPTION
    1. Determines your current branch.
    2. Lists all branches merged into `master`.
    3. If you’re sitting on a merged branch, checks out `master` first.
    4. Deletes each merged branch locally.

    May the Force be with your repo cleanup. #>

Push-Location -Path (git rev-parse --show-toplevel) 2>$null

$primaryBranch = "master"

$currentBranch = (git rev-parse --abbrev-ref HEAD).Trim()
Write-Host "Current branch is '$currentBranch'."

Write-Host "Pulling possible changes from origin to ensure latest data..."
git stash
git pull

$mergedBranches = git branch --merged |
        ForEach-Object { $_.Trim() } |
        Where-Object { -not ($_ -eq "$primaryBranch")} |
        ForEach-Object { $_.TrimStart('*').Trim() }

if (-not $mergedBranches) {
    Write-Host "No branches to delete. All clear!" -ForegroundColor Green
    Pop-Location
    return
}

if ($currentBranch -ne $primaryBranch -and $mergedBranches -contains $currentBranch) {
    Write-Host "You're on a merged branch ('$currentBranch'). Checking out '$primaryBranch'..." -ForegroundColor Yellow
    git checkout $primaryBranch
    $currentBranch = $primaryBranch
}

$branchesToDelete = $mergedBranches | Where-Object { $_ -ne $currentBranch }
foreach ($b in $branchesToDelete) {
    Write-Host "Deleting merged branch '$b'..." -ForegroundColor Cyan
    git branch -d $b
}
git stash pop

Write-Host "Clean up complete! May the Force keep your history tidy!" -ForegroundColor Green

Pop-Location