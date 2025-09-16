import {defineConfig} from 'vitest/config'
import react from '@vitejs/plugin-react'
import path from "path";

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
      '@components': path.resolve(__dirname, './src/components'),
      '@pages': path.resolve(__dirname, './src/pages'),
      '@hooks': path.resolve(__dirname, './src/hooks'),
      '@services': path.resolve(__dirname, './src/services'),
      '@utils': path.resolve(__dirname, './src/utils'),
      '@types': path.resolve(__dirname, './src/types'),
      '@styles': path.resolve(__dirname, './src/styles'),
    },
  },
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: './setupTests.ts',
    css: true,
    include: ['test/**/*.test.ts', 'test/**/*.test.tsx', 'test/**/*.spec.ts', 'test/**/*.spec.tsx'],
    coverage: {
      provider: 'v8',
      reporter: ['text', 'html', 'json', 'lcov'],
      include: ['src/**/*.{ts,tsx}'],
      exclude: [
        // Exclude files that are not relevant for coverage
        'src/main.tsx',
        'src/App.tsx',
      ]
    },
  },
})