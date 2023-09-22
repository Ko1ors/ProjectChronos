export {};

declare global {
    namespace NodeJS {
      interface ProcessEnv {
        SECRET_KEY: string;
        NODE_ENV: 'development' | 'production';
      }
    }
  }