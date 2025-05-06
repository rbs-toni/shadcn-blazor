import * as esbuild from 'esbuild'

await esbuild.build({
    entryPoints: [ './Scripts/rect.js' ],
    bundle: true,
    minify: true,
    target: 'es2022',
    format: 'esm',
    outfile: './wwwroot/modules/rect.min.js',
});