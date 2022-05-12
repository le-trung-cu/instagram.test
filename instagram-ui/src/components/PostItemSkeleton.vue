<template>
  <div class="flex flex-col aspect-[3/2] justify-center items-stretch tablet:flex-row m-auto w-full max-w-3xl">
    <div class="bg-white flex-grow tablet:basis-1/2">
      <div class="animate-pulse bg-slate-200 h-full"></div>
    </div>
    <div class="bg-white rounded-r tablet:basis-1/2">
      <div class="border shadow rounded-tr p-4 w-full mx-auto">
        <div class="animate-pulse flex space-x-4">
          <div class="rounded-full bg-slate-200 h-10 w-10"></div>
          <div class="flex-1 space-y-6 py-1">
            <div class="h-2 bg-slate-200 rounded"></div>
            <div class="space-y-3">
              <div class="grid grid-cols-3 gap-4">
                <div class="h-2 bg-slate-200 col-span-2"></div>
                <div class="h-2 bg-slate-200 col-span-1"></div>
              </div>
              <div class="h-2 bg-slate-200"></div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>