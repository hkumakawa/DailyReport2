# Copilot Instructions
---
name: copilot-instractions
description: |
  アプリケーション開発基本ルール
---

## 基本方針

- 回答は日本語で記述する。
- コード生成時は指定された技術要件を優先する。
- ビルド可能な状態を最終成果物とする。
- Git Commit、Push、ブランチ作成は行わない。

## 命名規則

- クラス名は PascalCase
- メソッド名は PascalCase
- プロパティ名は PascalCase
- インターフェース名は I + PascalCase
- 名前空間は PascalCase
- 変数名は camelCase
- 定数名は UPPER_SNAKE_CASE

## コーディングスタイル

- 波括弧は改行形式を採用し、インデントを正しく設定する。

例

public class Example
{
    private int exampleField;
}

- XMLコメントを重視する。
- XMLコメントは、クラス、フィールド、プロパティ、メソッドに付与する。
- メソッドのXMLコメントは概要だけでなく引数、戻り値、例外も記載する。
- 可読性を優先する。
- マジックナンバーを避ける。
- 可能な限り責務を分離する。

## テスト方針

- コードカバレッジを意識する。
- テストメソッド名は

  対象メソッド名_条件_期待結果

  とする。

- テストプロジェクト名は

  対象プロジェクト名.UnitTest

  とする。
  
