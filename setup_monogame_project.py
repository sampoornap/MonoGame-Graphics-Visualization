#!/usr/bin/env python3
"""
MonoGame Project Setup Script

Copies Content directory and C# source files from a downloaded MonoGame project
to a clean template project on your machine. This avoids cross-machine build issues.

Usage:
    python3 setup_monogame_project.py <source_project> <dest_project> [--template <template_path>]

Example:
    python3 setup_monogame_project.py ~/Downloads/SomeGame ~/projects/SomeGame
    python3 setup_monogame_project.py ~/Downloads/SomeGame ~/projects/SomeGame --template ~/projects/MonoGameTemplate
"""

import os
import sys
import shutil
import argparse
from pathlib import Path


def copy_content_directory(source_project, dest_project):
    """
    Copy the Content directory from source to destination, excluding bin/ and obj/
    """
    source_content = Path(source_project) / "Content"
    dest_content = Path(dest_project) / "Content"
    
    if not source_content.exists():
        print(f"⚠️  No Content directory found in {source_project}")
        return False
    
    # Remove existing Content directory if it exists
    if dest_content.exists():
        shutil.rmtree(dest_content)
        print(f"Removed existing Content directory at {dest_content}")
    
    # Copy with filtering
    def ignore_patterns(directory, files):
        """Ignore bin and obj directories"""
        return {f for f in files if f in ('bin', 'obj', '.DS_Store')}
    
    shutil.copytree(source_content, dest_content, ignore=ignore_patterns)
    print(f"✓ Copied Content directory to {dest_content}")
    return True


def copy_cs_files(source_project, dest_project, files_to_copy=None):
    """
    Copy C# source files from source to destination.
    Default: Game1.cs
    """
    if files_to_copy is None:
        files_to_copy = ['Game1.cs']
    
    source_path = Path(source_project)
    dest_path = Path(dest_project)
    
    copied_files = []
    for file in files_to_copy:
        source_file = source_path / file
        if source_file.exists():
            dest_file = dest_path / file
            shutil.copy2(source_file, dest_file)
            copied_files.append(file)
            print(f"✓ Copied {file}")
        else:
            print(f"⚠️  {file} not found in {source_project}")
    
    if copied_files:
        print(f"✓ Copied {len(copied_files)} C# source file(s)")
        return True
    return False


def copy_from_template(template_project, dest_project):
    """
    Initialize destination project from a clean template
    """
    template_path = Path(template_project)
    dest_path = Path(dest_project)
    
    if not template_path.exists():
        print(f"✗ Template project not found at {template_project}")
        return False
    
    # Copy the entire template, excluding Content, bin, obj, and specific files
    def ignore_patterns(directory, files):
        return {f for f in files if f in ('bin', 'obj', 'Content', '.DS_Store', '.vs')}
    
    if dest_path.exists():
        shutil.rmtree(dest_path)
    
    shutil.copytree(template_path, dest_path, ignore=ignore_patterns)
    print(f"✓ Initialized project from template: {template_path}")
    
    # Create empty Content directory
    (dest_path / "Content").mkdir(parents=True, exist_ok=True)
    print(f"✓ Created empty Content directory")
    
    return True


def main():
    parser = argparse.ArgumentParser(
        description="Copy MonoGame project assets and code to a clean template",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  # Copy Game1.cs and Content to existing project
  python3 setup_monogame_project.py ~/Downloads/MyGame ~/projects/MyGame
  
  # Initialize from template first, then copy
  python3 setup_monogame_project.py ~/Downloads/MyGame ~/projects/MyGame \\
    --template ~/projects/MonoGameTemplate
  
  # Copy additional files
  python3 setup_monogame_project.py ~/Downloads/MyGame ~/projects/MyGame \\
    --files Game1.cs Utils.cs Helpers.cs
        """
    )
    
    parser.add_argument("source", help="Path to the source MonoGame project")
    parser.add_argument("destination", help="Path to the destination project")
    parser.add_argument(
        "--template",
        help="Path to a clean MonoGame template project (will initialize destination from this)"
    )
    parser.add_argument(
        "--files",
        nargs='+',
        default=['Game1.cs'],
        help="C# files to copy (default: Game1.cs)"
    )
    parser.add_argument(
        "--no-content",
        action="store_true",
        help="Skip copying the Content directory"
    )
    parser.add_argument(
        "--no-code",
        action="store_true",
        help="Skip copying C# files"
    )
    
    args = parser.parse_args()
    
    # Validate source exists
    if not Path(args.source).exists():
        print(f"✗ Source project not found: {args.source}")
        sys.exit(1)
    
    source = Path(args.source).resolve()
    dest = Path(args.destination).resolve()
    
    print(f"\n📦 MonoGame Project Setup")
    print(f"Source: {source}")
    print(f"Destination: {dest}\n")
    
    # Initialize from template if provided
    if args.template:
        if not copy_from_template(args.template, dest):
            sys.exit(1)
    else:
        # Create destination if it doesn't exist
        dest.mkdir(parents=True, exist_ok=True)
    
    # Copy Content directory
    if not args.no_content:
        copy_content_directory(source, dest)
    
    # Copy C# files
    if not args.no_code:
        copy_cs_files(source, dest, args.files)
    
    print("\n✓ Project setup complete!")


if __name__ == "__main__":
    main()
