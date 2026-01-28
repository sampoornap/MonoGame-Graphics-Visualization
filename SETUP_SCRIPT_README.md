# MonoGame Project Setup Script

This script automates the process of copying game code and assets from a downloaded MonoGame project into a clean template project on your machine. This avoids common cross-machine build issues.

## Prerequisites

- Python 3.6+
- A clean MonoGame template project (optional, but recommended)

## Installation

1. Save the script to a convenient location (e.g., `~/bin/` or in your workspace)
2. Make it executable: `chmod +x setup_monogame_project.py`
3. (Optional) Create a symlink for easier access: `ln -s /path/to/setup_monogame_project.py ~/bin/setup-monogame`

## Quick Start

### Option 1: Copy to Existing Project (Simple)
```bash
python3 setup_monogame_project.py ~/Downloads/DownloadedGame ~/projects/MyGame
```

This copies:
- `Game1.cs` 
- `Content/` directory (excluding `bin/` and `obj/`)

### Option 2: Initialize from Template (Recommended)
First, create a clean MonoGame template by setting up one project manually, then:

```bash
python3 setup_monogame_project.py ~/Downloads/DownloadedGame ~/projects/MyGame \
  --template ~/projects/MonoGameTemplate
```

This:
1. Copies the template project structure to the destination
2. Copies `Game1.cs` from the downloaded game
3. Copies the `Content/` directory from the downloaded game

## Usage Examples

### Copy Game1.cs and Content only
```bash
python3 setup_monogame_project.py ~/Downloads/Game1 ~/projects/Game1
```

### Copy multiple C# files
```bash
python3 setup_monogame_project.py ~/Downloads/Game1 ~/projects/Game1 \
  --files Game1.cs Utils.cs Helpers.cs GameState.cs
```

### Copy Content but not code
```bash
python3 setup_monogame_project.py ~/Downloads/Game1 ~/projects/Game1 --no-code
```

### Copy code but not Content
```bash
python3 setup_monogame_project.py ~/Downloads/Game1 ~/projects/Game1 --no-content
```

### Initialize from template with custom files
```bash
python3 setup_monogame_project.py ~/Downloads/Game1 ~/projects/Game1 \
  --template ~/projects/MonoGameTemplate \
  --files Game1.cs Player.cs Enemy.cs
```

## Setting Up Your Template Project

1. Create a new MonoGame project using the MonoGame templates:
   ```bash
   dotnet new monogame -n MonoGameTemplate -o ~/projects/MonoGameTemplate
   ```

2. Build it once to ensure it works:
   ```bash
   cd ~/projects/MonoGameTemplate
   dotnet build
   dotnet run
   ```

3. Now you have a clean template to use with this script!

## What Gets Copied

### Content Directory
- Copies entire `Content/` folder
- **Excludes**: `bin/`, `obj/`, `.DS_Store`

### C# Files
- By default: `Game1.cs`
- Custom files via `--files` argument

### When Using Template
- All project files (`.csproj`, `Program.cs`, etc.)
- Excludes: `bin/`, `obj/`, `.vs/`, `Content/` (creates empty one)

## Command-Line Options

```
usage: setup_monogame_project.py [-h] [--template TEMPLATE] 
                                 [--files FILES [FILES ...]] 
                                 [--no-content] [--no-code]
                                 source destination

positional arguments:
  source                Path to the source MonoGame project
  destination           Path to the destination project

optional arguments:
  -h, --help            Show this help message
  --template TEMPLATE   Path to a clean MonoGame template project
  --files FILES ...     C# files to copy (default: Game1.cs)
  --no-content          Skip copying the Content directory
  --no-code             Skip copying C# files
```

## Troubleshooting

### "Template project not found"
- Verify the template path is correct
- Make sure you've created a clean template first

### "Content directory not found"
- The source project doesn't have a `Content/` folder
- Use `--no-content` to skip this
- Create the Content directory manually if needed

### Project won't build after copying
- Ensure your template project builds cleanly first
- Check that all referenced C# files were copied
- Verify the Content.mgcb file in the template points to correct paths
- Try cleaning: `dotnet clean && dotnet build`

## Tips & Best Practices

1. **Create your template once**: Build it carefully once, then reuse it
2. **Version your template**: Keep backups of your template for different MonoGame versions
3. **Test the template**: Always verify your template builds and runs before using it
4. **Copy incrementally**: If a game has many custom C# files, copy them step-by-step
5. **Check Content paths**: Some games may have Content organized differently; verify after copying

## Example Workflow

```bash
# Set up your template once
dotnet new monogame -n MonoGameTemplate -o ~/projects/MonoGameTemplate
cd ~/projects/MonoGameTemplate && dotnet build

# Now reuse it for every new game!
python3 setup_monogame_project.py ~/Downloads/Game1 ~/projects/Game1 \
  --template ~/projects/MonoGameTemplate

python3 setup_monogame_project.py ~/Downloads/Game2 ~/projects/Game2 \
  --template ~/projects/MonoGameTemplate

# Build and run
cd ~/projects/Game1 && dotnet run
```
