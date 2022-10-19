namespace DiscordBot.NetCore
{
    class EntryPoint
    {
        private Logger _logger { get; } = Logger.GetLogger(typeof(EntryPoint));
        public static Config.ConfigInfo _config { get; private set; }

        /// <summary>
        /// メイン実行メソッド (自動呼出し)
        /// 設定ファイルの読み出しとメインルーチン実行
        /// </summary>
        /// <param name="args">引数</param>
        /// <returns></returns>
        public static void Main(string[] args)
        {
            _config = Config.GetConfigJSON();
            if (null != _config.loglevel)
            {
                Logger.SetOutputLogLevel(_config.loglevel);
            }

            new MainLogic().MainAsync().GetAwaiter().GetResult();
        }
    }
}
