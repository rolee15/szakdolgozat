const HomePage = () => {
  return (
    <div className="mx-auto p-6 flex flex-col flex-grow pt-[12vh]">
      <h1 className="text-center text-6xl">漢字家</h1>
      <h1 className="text-center mb-36 text-6xl">Kanjika</h1>
      <p className="text-center text-3xl" lang="ja">
        漢字家へようこそ！
      </p>
      <p className="text-3xl font-bold text-center mb-12">Welcome to Kanjika!</p>
      <p className="text-center" lang="ja">
        日本語を学び、練習できる場所です。
      </p>
      <p className="text-center">Where you can learn and practice Japanese.</p>
    </div>
  );
};

export default HomePage;
