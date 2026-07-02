import os
import re
import argparse
import urllib.parse
from pathlib import Path

def build_index(root_dir):
    """Build a map for link resolution."""
    index = {}
    for root, _, files in os.walk(root_dir):
        if '.git' in root or '.obsidian' in root:
            continue
        for f in files:
            path = Path(root) / f
            if f.endswith('.md'):
                index[path.stem.lower()] = path
            else:
                index[path.name.lower()] = path
    return index

def process_file_obs2md(filepath, index):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    original_content = content
    current_dir = filepath.parent

    # Match [[Link|Display Text]] or [[Link]]
    def replacer(m):
        inner = m.group(1)
        
        if '|' in inner:
            link_target, display_text = inner.split('|', 1)
        else:
            link_target = inner
            display_text = inner
        
        # Look up in index
        basename = link_target.lower().strip()
        if basename in index:
            target_path = index[basename]
            # Calculate relative path
            try:
                rel_path = os.path.relpath(target_path, current_dir)
                # URL encode spaces and special chars, but keep slashes
                rel_path = urllib.parse.quote(rel_path.replace('\\', '/'), safe='/')
                return f"[{display_text}]({rel_path})"
            except ValueError:
                pass
        
        # If not found in index, fallback
        has_ext = '.' in link_target and len(link_target.split('.')[-1]) <= 4
        ext = '' if has_ext else '.md'
        encoded_target = urllib.parse.quote(link_target.strip()) + ext
        return f"[{display_text}]({encoded_target})"

    content = re.sub(r'\[\[(.*?)\]\]', replacer, content)

    if content != original_content:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(content)
        return True
    return False

def process_file_md2obs(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    original_content = content

    # Match [Display Text](path)
    # Be careful not to match image links ![Alt](path)
    def replacer(m):
        display_text = m.group(1)
        link_path = urllib.parse.unquote(m.group(2))
        
        # Ignore external links
        if link_path.startswith('http'):
            return m.group(0)
            
        path_obj = Path(link_path)
        if link_path.endswith('.md'):
            filename = path_obj.stem
        else:
            filename = path_obj.name
            
        if display_text == filename:
            return f"[[{filename}]]"
        else:
            return f"[[{filename}|{display_text}]]"

    content = re.sub(r'(?<!\!)\[(.*?)\]\((.*?)\)', replacer, content)

    if content != original_content:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(content)
        return True
    return False

def main():
    parser = argparse.ArgumentParser(description='Convert Obsidian and Standard Markdown links.')
    parser.add_argument('--mode', choices=['obs2md', 'md2obs'], required=True, help='Conversion mode')
    parser.add_argument('--dir', default='.', help='Directory to process')
    args = parser.parse_args()

    root_dir = Path(args.dir).resolve()
    print(f"Scanning directory: {root_dir}")

    if args.mode == 'obs2md':
        index = build_index(root_dir)
        print(f"Built index of {len(index)} markdown files.")
        
        changed_files = 0
        for root, _, files in os.walk(root_dir):
            for f in files:
                if f.endswith('.md'):
                    filepath = Path(root) / f
                    if process_file_obs2md(filepath, index):
                        changed_files += 1
                        
        print(f"Conversion complete. Modified {changed_files} files.")
    
    elif args.mode == 'md2obs':
        changed_files = 0
        for root, _, files in os.walk(root_dir):
            for f in files:
                if f.endswith('.md'):
                    filepath = Path(root) / f
                    if process_file_md2obs(filepath):
                        changed_files += 1
                        
        print(f"Conversion complete. Modified {changed_files} files.")

if __name__ == '__main__':
    main()
