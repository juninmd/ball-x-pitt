import sys
import glob

def check_brace_balance(filepath):
    with open(filepath, 'r') as f:
        content = f.read()

    stack = []
    for i, char in enumerate(content):
        if char == '{':
            stack.append(i)
        elif char == '}':
            if not stack:
                print(f"Error: Unmatched '}}' in {filepath} at index {i}")
                return False
            stack.pop()

    if stack:
        print(f"Error: Unmatched '{{' in {filepath} at indices {stack}")
        return False

    return True

all_good = True
for f in glob.glob("Assets/Scripts/**/*.cs", recursive=True):
    if not check_brace_balance(f):
        all_good = False

if all_good:
    print("Syntax check passed.")
    sys.exit(0)
else:
    sys.exit(1)
