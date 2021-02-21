# SKY PRIME PIXELS
Unity 2D弾幕シューティングゲーム  
公開ページ => 近日公開予定  
デモ動画 => https://youtu.be/rLIN28-TdCY

# 目次
* [概要](#概要)
* [システム](#システム)
* [製作](#製作)

# 概要
タイトル：SKY PRIME PIXELS  
ジャンル：弾幕シューティング  
「ピクセル」をイメージしたデザインの弾幕シューティングゲーム  
1面のみの構成となっており、カジュアルに遊ぶことのできるゲームとなっている  
開発にはゲームエンジンUnityを使用  
データ駆動型のプログラムを意識した作りとなっている


# システム
## [ゲームルール]
道中の敵を倒しつつ進み、最後のボスを倒すことでゲームクリア  
自機中心の判定に被弾すると、残機が一つ減る  
残機が全てなくなるとゲームオーバー

## [操作]
* [移動]  
上下左右と斜めの8方向に移動可能  
* [ショット]  
ショットボタンを押し続けることで、ショットを撃つことができる  
ゲーム開始時に「プライマリショット」と「セカンダリショット」を3種類のショットから選択することができる  
通常次は「プライマリショット」を撃ち、低速移動中は「セカンダリショット」を撃つシステムとなっている  
ショットの種類は「FourWay」「Spread」「Homing」となっている  
* [低速移動]  
低速移動ボタンを押している間はプレイヤーの移動速度が若干低下する低速移動状態となる  
低速移動状態の間は「セカンダリショット」を撃つようになる
* [ボンバー]  
ボンバーを使用することで画面上の敵弾を消すことができ、効果時間中は無敵となる
使用には画面下部のボンバーゲージを一つ消費する必要がある  

## [スコア]
* スコア取得
敵にプレイヤーの撃った弾を当てて撃破することで、スコアを取得することができる  
取得できるスコアは撃破した敵の種類により異なる  
また、後述のランクが倍率として掛かるといったシステムになっている

## [ランク]
* ランクについて  
このゲームにはランクシステムが存在しており、ランクの上昇に応じて敵の放つ弾の速度が変化する  
それと同時に、敵を倒した際の獲得スコアに倍率が掛かるようになっている
* ランクの上昇  
ランクは時間の経過と敵の撃破によって上昇する  
ランク上昇の上限は100となっている
* ランクの減少  
ランクはボムの使用、被弾によって減少する使用となっている  
ボムの使用ではランクが2/1減少  
被弾ではランクが3/1減少


# 製作
### 混沌の蛙 F-Rog [[Twitter]](https://twitter.com/CF_Frog)  
### クレジット:
* [開発]
    * Unity
* [フォント]
    * 8bit Operator
    * Pixel M Plus
* [効果音]
    * 効果音ラボ 様
    * ZapSplat 様
* [BGM・ジングル]
    * HURT RECORD 様
        * 宇宙八目（おメガネ）
    * DOVA-SYNDROME 様
        * ターゲットNo.8（かずち）
        * Ultimate Design（FLASH☆BEAT）
        * GraVity StrikeR（MAKOOTO）
        * Light Trail（FLASH☆BEAT）
        * coolness（カワサキヤスヒロ）
* [Special Thanks]
    * 株式会社カンム 様
