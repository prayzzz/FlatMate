# Primitive Types

## bool?

| default    | null   |
|:-----------|:-------|
| null       | ignore |
| true/false | set    |

## string?

| default    | null   |
|:-----------|:-------|
| null       | ignore |
| ""         | set    |
| "any"      | set    |

## int?

| default    | null   |
|:---------- |:-------|
| null       | ignore |
| any number | set    |

# Complex Types


## custom complex types

| default          | default instance |
|:-----------------|:-----------------|
| null             | clear            |
| default-instance | ignore           |
| any instance     | set              |
