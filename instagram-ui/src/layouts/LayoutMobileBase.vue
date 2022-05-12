<script setup lang="ts">
import BottomNavigationBar from '@/components/mobile/BottomNavigationBar.vue';
</script>

<template>
  <div id="mobileLayoutBase">
    <aside class="fixed top-0 left-0 right-0 h-11 z-30">
      <slot name="appBar"></slot>
    </aside>
    <section id="baseModalReferenceId" class="pt-11 pb-14">
      <slot name="body">Body</slot>
    </section>
    <header id="bottomNavigationBar" class="fixed bottom-0 left-0 h-11 right-0 z-30">
      <BottomNavigationBar class="h-11"/>
    </header>
  </div>
</template>