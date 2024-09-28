# Makefile for building IPK Project 2 with dotnet build

# Variables
PROJECT_NAME = project2.csproj
SRC_DIR = src
OUT_DIR = bin
OUT_NAME = project2
DST_NAME = ipk-sniffer

# Phony targets
.PHONY: all clean

# Default target
all: $(OUT_NAME)

# Compiling target
$(OUT_NAME): $(wildcard $(SRC_DIR)/*.cs)
	@echo "======================="
	@echo "Building with dotnet..."
	@echo "======================="
	# -r linux-x64 
	dotnet publish /p:DebugType=None /p:DebugSymbols=false -c Release -p:PublishSingleFile=true --self-contained true -o . $(PROJECT_NAME)
	@echo "======================="
	@echo "Moving compiled files..."
	@echo "======================="
	mv $(OUT_NAME) $(DST_NAME)
	chmod +x $(DST_NAME)


# Clean target
clean:
	@echo "======================="
	@echo "Removing $(OUT_DIR) and $(DST_NAME)..."
	@echo "======================="
	rm -rf $(OUT_DIR) $(DST_NAME)

# Rebuild target
re: clean all
